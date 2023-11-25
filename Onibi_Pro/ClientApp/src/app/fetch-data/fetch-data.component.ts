import { Component } from '@angular/core';
import { WeatherForecast, WeatherForecastClient } from '../api/api';
import { take, tap } from 'rxjs';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

  constructor(private readonly client: WeatherForecastClient) {
    client
      .weatherForecast()
      .pipe(
        take(1),
        tap((result) => (this.forecasts = result))
      )
      .subscribe();
  }
}
