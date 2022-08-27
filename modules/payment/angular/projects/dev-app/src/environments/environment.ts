import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl: 'http://localhost:4200/',
    name: 'PaymentManagement',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44373',
    redirectUri: baseUrl,
    clientId: 'PaymentManagement_App',
    responseType: 'code',
    scope: 'offline_access PaymentManagement role email openid profile',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44373',
      rootNamespace: 'Full.Abp.PaymentManagement',
    },
    PaymentManagement: {
      url: 'https://localhost:44361',
      rootNamespace: 'Full.Abp.PaymentManagement',
    },
  },
} as Environment;
