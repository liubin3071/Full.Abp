import { Component, OnInit } from '@angular/core';
import { PaymentManagementService } from '../services/payment-management.service';

@Component({
  selector: 'lib-payment-management',
  template: ` <p>payment-management works!</p> `,
  styles: [],
})
export class PaymentManagementComponent implements OnInit {
  constructor(private service: PaymentManagementService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
