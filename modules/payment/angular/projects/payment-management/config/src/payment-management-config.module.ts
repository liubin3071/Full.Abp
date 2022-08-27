import { ModuleWithProviders, NgModule } from '@angular/core';
import { PAYMENT_MANAGEMENT_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class PaymentManagementConfigModule {
  static forRoot(): ModuleWithProviders<PaymentManagementConfigModule> {
    return {
      ngModule: PaymentManagementConfigModule,
      providers: [PAYMENT_MANAGEMENT_ROUTE_PROVIDERS],
    };
  }
}
