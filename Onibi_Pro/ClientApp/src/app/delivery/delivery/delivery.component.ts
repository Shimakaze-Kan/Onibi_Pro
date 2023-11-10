import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDeliveryComponent } from '../confirm-delivery/confirm-delivery.component';
import { RequestSuppliesComponent } from '../request-supplies/request-supplies.component';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.scss'],
})
export class DeliveryComponent implements AfterViewInit {
  delivery: Array<IDeliveryItem> = [
    {
      code: 'D001',
      from: 'Warehouse A',
      to: 'Customer X',
      status: 'Shipped',
    },
    {
      code: 'D002',
      from: 'Warehouse B',
      to: 'Customer Y',
      status: 'Delivered',
    },
    {
      code: 'D003',
      from: 'Warehouse C',
      to: 'Customer Z',
      status: 'In Transit',
    },
    {
      code: 'D004',
      from: 'Warehouse A',
      to: 'Customer P',
      status: 'Pending',
    },
    {
      code: 'D005',
      from: 'Warehouse D',
      to: 'Customer Q',
      status: 'Delivered',
    },
    {
      code: 'D006',
      from: 'Warehouse E',
      to: 'Customer R',
      status: 'Shipped',
    },
    {
      code: 'D007',
      from: 'Warehouse F',
      to: 'Customer S',
      status: 'In Transit',
    },
    {
      code: 'D008',
      from: 'Warehouse G',
      to: 'Customer T',
      status: 'Delivered',
    },
    {
      code: 'D009',
      from: 'Warehouse H',
      to: 'Customer U',
      status: 'Pending',
    },
    {
      code: 'D010',
      from: 'Warehouse I',
      to: 'Customer V',
      status: 'Shipped',
    },
  ];

  dataSource = new MatTableDataSource<IDeliveryItem>(this.delivery);
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  displayedColumns = ['code', 'from', 'to', 'status'];

  constructor(private readonly dialog: MatDialog) {}

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  openConfirmDeliveryDialog() {
    const dialogRef = this.dialog.open(ConfirmDeliveryComponent, {
      maxWidth: '750px',
      width: '600px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
    });
  }

  openRequestSuppliesDialog() {
    const dialogRef = this.dialog.open(RequestSuppliesComponent, {
      maxWidth: '750px',
      width: '600px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
    });
  }
}

interface IDeliveryItem {
  code: string;
  from: string;
  to: string;
  status: string;
}
