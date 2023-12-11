import { gql } from 'apollo-angular';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: { input: any; output: any; }
  /** The `Long` scalar type represents non-fractional signed whole 64-bit numeric values. Long can represent values between -(2^63) and 2^63 - 1. */
  Long: { input: any; output: any; }
};

export enum ApplyPolicy {
  AfterResolver = 'AFTER_RESOLVER',
  BeforeResolver = 'BEFORE_RESOLVER',
  Validation = 'VALIDATION'
}

export type DateTimeOperationFilterInput = {
  eq?: InputMaybe<Scalars['DateTime']['input']>;
  gt?: InputMaybe<Scalars['DateTime']['input']>;
  gte?: InputMaybe<Scalars['DateTime']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  lt?: InputMaybe<Scalars['DateTime']['input']>;
  lte?: InputMaybe<Scalars['DateTime']['input']>;
  neq?: InputMaybe<Scalars['DateTime']['input']>;
  ngt?: InputMaybe<Scalars['DateTime']['input']>;
  ngte?: InputMaybe<Scalars['DateTime']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  nlt?: InputMaybe<Scalars['DateTime']['input']>;
  nlte?: InputMaybe<Scalars['DateTime']['input']>;
};

export type DpdShipmentViewModel = {
  __typename?: 'DpdShipmentViewModel';
  allegroOrderNumber?: Maybe<Scalars['String']['output']>;
  createdAt: Scalars['DateTime']['output'];
  shipmentNumber?: Maybe<Scalars['String']['output']>;
  trackingNumber?: Maybe<Scalars['String']['output']>;
};

export type DpdShipmentViewModelFilterInput = {
  allegroOrderNumber?: InputMaybe<StringOperationFilterInput>;
  and?: InputMaybe<Array<DpdShipmentViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  or?: InputMaybe<Array<DpdShipmentViewModelFilterInput>>;
  shipmentNumber?: InputMaybe<StringOperationFilterInput>;
  trackingNumber?: InputMaybe<StringOperationFilterInput>;
};

export type InpostShipmentViewModel = {
  __typename?: 'InpostShipmentViewModel';
  allegroOrderNumber?: Maybe<Scalars['String']['output']>;
  createdAt: Scalars['DateTime']['output'];
  shipmentNumber?: Maybe<Scalars['String']['output']>;
  trackingNumber?: Maybe<Scalars['String']['output']>;
};

export type InpostShipmentViewModelFilterInput = {
  allegroOrderNumber?: InputMaybe<StringOperationFilterInput>;
  and?: InputMaybe<Array<InpostShipmentViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  or?: InputMaybe<Array<InpostShipmentViewModelFilterInput>>;
  shipmentNumber?: InputMaybe<StringOperationFilterInput>;
  trackingNumber?: InputMaybe<StringOperationFilterInput>;
};

export type IntegratorQuery = {
  __typename?: 'IntegratorQuery';
  dpdShipments?: Maybe<Array<Maybe<DpdShipmentViewModel>>>;
  inpostShipments?: Maybe<Array<Maybe<InpostShipmentViewModel>>>;
  invoices?: Maybe<Array<Maybe<InvoiceViewModel>>>;
  shipments?: Maybe<Array<Maybe<ShipmentViewModel>>>;
};


export type IntegratorQueryDpdShipmentsArgs = {
  where?: InputMaybe<DpdShipmentViewModelFilterInput>;
};


export type IntegratorQueryInpostShipmentsArgs = {
  where?: InputMaybe<InpostShipmentViewModelFilterInput>;
};


export type IntegratorQueryInvoicesArgs = {
  where?: InputMaybe<InvoiceViewModelFilterInput>;
};


export type IntegratorQueryShipmentsArgs = {
  where?: InputMaybe<ShipmentViewModelFilterInput>;
};

export type InvoiceViewModel = {
  __typename?: 'InvoiceViewModel';
  allegroOrderNumber?: Maybe<Scalars['String']['output']>;
  createdAt: Scalars['DateTime']['output'];
  invoiceId: Scalars['Long']['output'];
  invoiceNumber?: Maybe<Scalars['String']['output']>;
};

export type InvoiceViewModelFilterInput = {
  allegroOrderNumber?: InputMaybe<StringOperationFilterInput>;
  and?: InputMaybe<Array<InvoiceViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  invoiceId?: InputMaybe<LongOperationFilterInput>;
  invoiceNumber?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<InvoiceViewModelFilterInput>>;
};

export type LongOperationFilterInput = {
  eq?: InputMaybe<Scalars['Long']['input']>;
  gt?: InputMaybe<Scalars['Long']['input']>;
  gte?: InputMaybe<Scalars['Long']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Long']['input']>>>;
  lt?: InputMaybe<Scalars['Long']['input']>;
  lte?: InputMaybe<Scalars['Long']['input']>;
  neq?: InputMaybe<Scalars['Long']['input']>;
  ngt?: InputMaybe<Scalars['Long']['input']>;
  ngte?: InputMaybe<Scalars['Long']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Long']['input']>>>;
  nlt?: InputMaybe<Scalars['Long']['input']>;
  nlte?: InputMaybe<Scalars['Long']['input']>;
};

export type ShipmentViewModel = {
  __typename?: 'ShipmentViewModel';
  allegroOrderNumber?: Maybe<Scalars['String']['output']>;
  createdAt: Scalars['DateTime']['output'];
  shipmentNumber?: Maybe<Scalars['String']['output']>;
  shipmentProvider?: Maybe<Scalars['String']['output']>;
  trackingNumber?: Maybe<Scalars['String']['output']>;
};

export type ShipmentViewModelFilterInput = {
  allegroOrderNumber?: InputMaybe<StringOperationFilterInput>;
  and?: InputMaybe<Array<ShipmentViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  or?: InputMaybe<Array<ShipmentViewModelFilterInput>>;
  shipmentNumber?: InputMaybe<StringOperationFilterInput>;
  shipmentProvider?: InputMaybe<StringOperationFilterInput>;
  trackingNumber?: InputMaybe<StringOperationFilterInput>;
};

export type StringOperationFilterInput = {
  and?: InputMaybe<Array<StringOperationFilterInput>>;
  contains?: InputMaybe<Scalars['String']['input']>;
  endsWith?: InputMaybe<Scalars['String']['input']>;
  eq?: InputMaybe<Scalars['String']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  ncontains?: InputMaybe<Scalars['String']['input']>;
  nendsWith?: InputMaybe<Scalars['String']['input']>;
  neq?: InputMaybe<Scalars['String']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  nstartsWith?: InputMaybe<Scalars['String']['input']>;
  or?: InputMaybe<Array<StringOperationFilterInput>>;
  startsWith?: InputMaybe<Scalars['String']['input']>;
};
