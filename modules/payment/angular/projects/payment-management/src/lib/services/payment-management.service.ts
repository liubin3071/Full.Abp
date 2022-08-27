import { Injectable } from '@angular/core';
import { RestService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class PaymentManagementService {
  apiName = 'PaymentManagement';

  constructor(private restService: RestService) {}

  sample() {
    return this.restService.request<void, any>(
      { method: 'GET', url: '/api/PaymentManagement/sample' },
      { apiName: this.apiName }
    );
  }
}
