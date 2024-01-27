import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  Renderer2,
} from '@angular/core';
import { Chart } from 'chart.js';
import { NgChartsModule } from 'ng2-charts';
import { map, tap } from 'rxjs';
import {
  GetIngredientStatisticsResponse,
  GetTopMenuItemsResponse,
  StatisticsClient,
} from '../app/api/api';
import { CommonModule } from '@angular/common';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss'],
  imports: [
    NgChartsModule,
    CommonModule,
    MatDividerModule,
    MatProgressSpinnerModule,
  ],
  standalone: true,
})
export class StatisticsComponent implements OnInit, AfterViewInit {
  ingredienStatisticsChart: unknown;
  topMenuItemsCharts: Array<unknown> = [];
  loading = false;

  constructor(
    private readonly client: StatisticsClient,
    private renderer: Renderer2,
    private elRef: ElementRef
  ) {}

  ngOnInit(): void {
    this.loading = true;

    this.client
      .ingredientStatistics()
      .pipe(
        tap((ingredients) => {
          this.createIngredientChart(ingredients);
        })
      )
      .subscribe();
  }

  ngAfterViewInit(): void {
    this.client
      .topMenuItems()
      .pipe(
        map((menuItems) => {
          const groupedData = menuItems.reduce((acc, obj) => {
            const restaurantId = obj.restaurantId || '';

            if (!acc[restaurantId]) {
              acc[restaurantId] = [];
            }

            acc[restaurantId].push(obj);

            return acc;
          }, {} as { [key: string]: GetTopMenuItemsResponse[] });

          return Object.values(groupedData);
        }),
        tap((grouped) => {
          const parentContainer = this.elRef.nativeElement;

          let topMenuItemsContainer =
            this.renderer.selectRootElement('#top-menu-items');
          if (!topMenuItemsContainer) {
            topMenuItemsContainer = this.renderer.createElement('div');
            this.renderer.setAttribute(
              topMenuItemsContainer,
              'id',
              'top-menu-items'
            );
            this.renderer.appendChild(parentContainer, topMenuItemsContainer);
          }

          for (const restaurant of grouped) {
            const itemCollection = restaurant as Array<GetTopMenuItemsResponse>;
            const restaurantId = itemCollection[0].restaurantId;
            const canvasContainer = this.renderer.createElement('div');
            this.renderer.addClass(canvasContainer, 'chart-container');

            const h5 = this.renderer.createElement('h5');
            const text = this.renderer.createText(
              `The Top 5 Most Frequently Purchased Menu Items at Restaurant ${restaurantId}`
            );
            this.renderer.appendChild(h5, text);
            this.renderer.addClass(h5, 'text-center');

            const canvas = this.renderer.createElement('canvas');

            this.renderer.appendChild(canvasContainer, h5);
            this.renderer.appendChild(canvasContainer, canvas);
            this.renderer.appendChild(topMenuItemsContainer, canvasContainer);

            const ctx = canvas.getContext('2d');

            const labels = itemCollection.map((x) => x.menuItemName);
            const data = itemCollection.map((x) => x.ordersCount);
            const colors = this.generatePastelAndDarkColors(
              itemCollection.length
            );

            new Chart(ctx, {
              type: 'doughnut',
              data: {
                labels: labels,
                datasets: [
                  {
                    label: `RestaurantId: ${restaurantId}`,
                    data: data,
                    backgroundColor: colors.background,
                    borderColor: colors.border,
                    hoverOffset: 4,
                  },
                ],
              },
            });
          }
          this.loading = false;
        })
      )
      .subscribe();
  }

  private createIngredientChart(
    ingredientData: Array<GetIngredientStatisticsResponse>
  ): void {
    const canvas = document.getElementById(
      'ingredient-statistics'
    ) as HTMLCanvasElement;
    const ctx = canvas.getContext('2d')!;

    const labels = ingredientData.map(
      (ingredient) => ingredient.ingredientName || ''
    );
    const data = ingredientData.map(
      (ingredient) => ingredient.totalQuantity || 0
    );

    const colors = this.generatePastelAndDarkColors(ingredientData.length);

    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: labels,
        datasets: [
          {
            label: 'Total Units',
            data: data,
            backgroundColor: colors.background,
            borderColor: colors.border,
            borderWidth: 1,
          },
        ],
      },
      options: {
        responsive: true,
        scales: {
          y: {
            beginAtZero: true,
          },
        },
        indexAxis: 'y',
      },
    });
  }

  private generatePastelAndDarkColors(count: number): {
    background: string[];
    border: string[];
  } {
    const colors: { background: string[]; border: string[] } = {
      background: [],
      border: [],
    };

    for (let i = 0; i < count; i++) {
      const hue = (360 / count) * i;
      const pastelColor = `hsl(${hue}, 70%, 80%)`;
      const darkColor = `hsl(${hue}, 70%, 60%)`;

      colors.background.push(pastelColor);
      colors.border.push(darkColor);
    }

    return colors;
  }
}
