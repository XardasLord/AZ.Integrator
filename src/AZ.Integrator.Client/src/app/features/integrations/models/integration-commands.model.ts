export interface AddErliIntegrationCommand {
  sourceSystemId: string;
  apiKey: string;
  displayName: string;
}

export interface AddInpostIntegrationCommand {
  organizationId: number;
  accessToken: string;
  displayName: string;
  senderName: string;
  senderCompanyName: string;
  senderFirstName: string;
  senderLastName: string;
  senderEmail: string;
  senderPhone: string;
  senderAddressStreet: string;
  senderAddressBuildingNumber: string;
  senderAddressCity: string;
  senderAddressPostCode: string;
  senderAddressCountryCode: string;
}

export interface AddShopifyIntegrationCommand {
  sourceSystemId: string;
  apiUrl: string;
  adminApiToken: string;
  displayName: string;
}

export interface AddFakturowniaIntegrationCommand {
  sourceSystemId: string;
  apiKey: string;
  apiUrl: string;
  displayName: string;
}

export interface AddDpdIntegrationCommand {
  login: number;
  password: string;
  masterFid: number;
  senderName: string;
  senderCompany: string;
  senderEmail: string;
  senderPhone: string;
  senderAddress: string;
  senderAddressCity: string;
  senderAddressPostCode: string;
  senderAddressCountryCode: string;
}

export interface UpdateIntegrationCommand {
  sourceSystemId: string;
  isEnabled: boolean;
}
