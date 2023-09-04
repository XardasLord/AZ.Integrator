import { TestModel } from './test.model';

export interface GetTestResponseModel {
  logCount: number;
  logs: TestModel[];
}
