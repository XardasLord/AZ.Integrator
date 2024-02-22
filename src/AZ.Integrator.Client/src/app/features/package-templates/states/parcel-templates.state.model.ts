import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';

export interface ParcelTemplatesStateModel {
  restQuery: RestQueryVo;
  restQueryResponse: RestQueryResponse<GetOfferSignaturesResponse>;
  signatures: string[];
}

export interface GetOfferSignaturesResponse {
  signatures: string[];
  count: number;
  totalCount: number;
}
