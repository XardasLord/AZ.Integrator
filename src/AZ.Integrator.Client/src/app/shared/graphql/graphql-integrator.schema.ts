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
  createdAt: Scalars['DateTime']['output'];
  externalOrderNumber?: Maybe<Scalars['String']['output']>;
  shipmentNumber?: Maybe<Scalars['String']['output']>;
};

export type DpdShipmentViewModelFilterInput = {
  and?: InputMaybe<Array<DpdShipmentViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  externalOrderNumber?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<DpdShipmentViewModelFilterInput>>;
  shipmentNumber?: InputMaybe<StringOperationFilterInput>;
};

export type FloatOperationFilterInput = {
  eq?: InputMaybe<Scalars['Float']['input']>;
  gt?: InputMaybe<Scalars['Float']['input']>;
  gte?: InputMaybe<Scalars['Float']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Float']['input']>>>;
  lt?: InputMaybe<Scalars['Float']['input']>;
  lte?: InputMaybe<Scalars['Float']['input']>;
  neq?: InputMaybe<Scalars['Float']['input']>;
  ngt?: InputMaybe<Scalars['Float']['input']>;
  ngte?: InputMaybe<Scalars['Float']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Float']['input']>>>;
  nlt?: InputMaybe<Scalars['Float']['input']>;
  nlte?: InputMaybe<Scalars['Float']['input']>;
};

export type InpostShipmentViewModel = {
  __typename?: 'InpostShipmentViewModel';
  createdAt: Scalars['DateTime']['output'];
  externalOrderNumber?: Maybe<Scalars['String']['output']>;
  shipmentNumber?: Maybe<Scalars['String']['output']>;
};

export type InpostShipmentViewModelFilterInput = {
  and?: InputMaybe<Array<InpostShipmentViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  externalOrderNumber?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<InpostShipmentViewModelFilterInput>>;
  shipmentNumber?: InputMaybe<StringOperationFilterInput>;
};

export type IntOperationFilterInput = {
  eq?: InputMaybe<Scalars['Int']['input']>;
  gt?: InputMaybe<Scalars['Int']['input']>;
  gte?: InputMaybe<Scalars['Int']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Int']['input']>>>;
  lt?: InputMaybe<Scalars['Int']['input']>;
  lte?: InputMaybe<Scalars['Int']['input']>;
  neq?: InputMaybe<Scalars['Int']['input']>;
  ngt?: InputMaybe<Scalars['Int']['input']>;
  ngte?: InputMaybe<Scalars['Int']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Int']['input']>>>;
  nlt?: InputMaybe<Scalars['Int']['input']>;
  nlte?: InputMaybe<Scalars['Int']['input']>;
};

export type IntegratorQuery = {
  __typename?: 'IntegratorQuery';
  dpdShipments?: Maybe<Array<Maybe<DpdShipmentViewModel>>>;
  inpostShipments?: Maybe<Array<Maybe<InpostShipmentViewModel>>>;
  invoices?: Maybe<Array<Maybe<InvoiceViewModel>>>;
  shipments?: Maybe<Array<Maybe<ShipmentViewModel>>>;
  stocks?: Maybe<Array<Maybe<StockViewModel>>>;
  tagParcelTemplates?: Maybe<Array<Maybe<TagParcelTemplateViewModel>>>;
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


export type IntegratorQueryStocksArgs = {
  where?: InputMaybe<StockViewModelFilterInput>;
};


export type IntegratorQueryTagParcelTemplatesArgs = {
  where?: InputMaybe<TagParcelTemplateViewModelFilterInput>;
};

export type InvoiceViewModel = {
  __typename?: 'InvoiceViewModel';
  createdAt: Scalars['DateTime']['output'];
  externalOrderNumber?: Maybe<Scalars['String']['output']>;
  invoiceId: Scalars['Long']['output'];
  invoiceNumber?: Maybe<Scalars['String']['output']>;
};

export type InvoiceViewModelFilterInput = {
  and?: InputMaybe<Array<InvoiceViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  externalOrderNumber?: InputMaybe<StringOperationFilterInput>;
  invoiceId?: InputMaybe<LongOperationFilterInput>;
  invoiceNumber?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<InvoiceViewModelFilterInput>>;
};

export type ListFilterInputTypeOfStockLogViewModelFilterInput = {
  all?: InputMaybe<StockLogViewModelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<StockLogViewModelFilterInput>;
  some?: InputMaybe<StockLogViewModelFilterInput>;
};

export type ListFilterInputTypeOfTagParcelViewModelFilterInput = {
  all?: InputMaybe<TagParcelViewModelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<TagParcelViewModelFilterInput>;
  some?: InputMaybe<TagParcelViewModelFilterInput>;
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
  createdAt: Scalars['DateTime']['output'];
  externalOrderNumber?: Maybe<Scalars['String']['output']>;
  shipmentNumber?: Maybe<Scalars['String']['output']>;
  shipmentProvider?: Maybe<Scalars['String']['output']>;
};

export type ShipmentViewModelFilterInput = {
  and?: InputMaybe<Array<ShipmentViewModelFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  externalOrderNumber?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ShipmentViewModelFilterInput>>;
  shipmentNumber?: InputMaybe<StringOperationFilterInput>;
  shipmentProvider?: InputMaybe<StringOperationFilterInput>;
};

export type StockLogViewModel = {
  __typename?: 'StockLogViewModel';
  changeQuantity: Scalars['Int']['output'];
  createdAt: Scalars['DateTime']['output'];
  createdBy?: Maybe<Scalars['String']['output']>;
  id: Scalars['Int']['output'];
  packageCode?: Maybe<Scalars['String']['output']>;
};

export type StockLogViewModelFilterInput = {
  and?: InputMaybe<Array<StockLogViewModelFilterInput>>;
  changeQuantity?: InputMaybe<IntOperationFilterInput>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  createdBy?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  or?: InputMaybe<Array<StockLogViewModelFilterInput>>;
  packageCode?: InputMaybe<StringOperationFilterInput>;
};

export type StockViewModel = {
  __typename?: 'StockViewModel';
  logs?: Maybe<Array<Maybe<StockLogViewModel>>>;
  packageCode?: Maybe<Scalars['String']['output']>;
  quantity: Scalars['Int']['output'];
};

export type StockViewModelFilterInput = {
  and?: InputMaybe<Array<StockViewModelFilterInput>>;
  logs?: InputMaybe<ListFilterInputTypeOfStockLogViewModelFilterInput>;
  or?: InputMaybe<Array<StockViewModelFilterInput>>;
  packageCode?: InputMaybe<StringOperationFilterInput>;
  quantity?: InputMaybe<IntOperationFilterInput>;
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

export type TagParcelTemplateViewModel = {
  __typename?: 'TagParcelTemplateViewModel';
  parcels?: Maybe<Array<Maybe<TagParcelViewModel>>>;
  tag?: Maybe<Scalars['String']['output']>;
  tenantId?: Maybe<Scalars['String']['output']>;
};

export type TagParcelTemplateViewModelFilterInput = {
  and?: InputMaybe<Array<TagParcelTemplateViewModelFilterInput>>;
  or?: InputMaybe<Array<TagParcelTemplateViewModelFilterInput>>;
  parcels?: InputMaybe<ListFilterInputTypeOfTagParcelViewModelFilterInput>;
  tag?: InputMaybe<StringOperationFilterInput>;
  tenantId?: InputMaybe<StringOperationFilterInput>;
};

export type TagParcelViewModel = {
  __typename?: 'TagParcelViewModel';
  height: Scalars['Float']['output'];
  id: Scalars['Int']['output'];
  length: Scalars['Float']['output'];
  tag?: Maybe<Scalars['String']['output']>;
  tenantId?: Maybe<Scalars['String']['output']>;
  weight: Scalars['Float']['output'];
  width: Scalars['Float']['output'];
};

export type TagParcelViewModelFilterInput = {
  and?: InputMaybe<Array<TagParcelViewModelFilterInput>>;
  height?: InputMaybe<FloatOperationFilterInput>;
  id?: InputMaybe<IntOperationFilterInput>;
  length?: InputMaybe<FloatOperationFilterInput>;
  or?: InputMaybe<Array<TagParcelViewModelFilterInput>>;
  tag?: InputMaybe<StringOperationFilterInput>;
  tenantId?: InputMaybe<StringOperationFilterInput>;
  weight?: InputMaybe<FloatOperationFilterInput>;
  width?: InputMaybe<FloatOperationFilterInput>;
};
