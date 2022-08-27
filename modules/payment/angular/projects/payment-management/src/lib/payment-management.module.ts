import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { PaymentManagementComponent } from './components/payment-management.component';
import { PaymentManagementRoutingModule } from './payment-management-routing.module';

@NgModule({
  declarations: [PaymentManagementComponent],
  imports: [CoreModule, ThemeSharedModule, PaymentManagementRoutingModule],
  exports: [PaymentManagementComponent],
})
export class PaymentManagementModule {
  static forChild(): ModuleWithProviders<PaymentManagementModule> {
    return {
      ngModule: PaymentManagementModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<PaymentManagementModule> {
    return new LazyModuleFactory(PaymentManagementModule.forChild());
  }
}
