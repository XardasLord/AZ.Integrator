import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap, throwError } from 'rxjs';
import { TestStateModel } from './test.state.model';
import { TestModel } from '../models/test.model';
import { TestService } from '../services/test.service';
import { RestQueryVo } from '../../../shared/models/pagination/rest.query';
import { RestQueryResponse } from '../../../shared/models/pagination/rest.response';
import { ChangePage, Load } from './test.action';

const TEST_STATE_TOKEN = new StateToken<TestStateModel>('test');

@State<TestStateModel>({
  name: TEST_STATE_TOKEN,
  defaults: {
    restQuery: new RestQueryVo(),
    restQueryResponse: new RestQueryResponse<TestModel[]>(),
  },
})
@Injectable()
export class TestState {
  constructor(private logsService: TestService) {}

  @Selector([TEST_STATE_TOKEN])
  static getLogs(state: TestStateModel): TestModel[] {
    return state.restQueryResponse.result;
  }

  @Selector([TEST_STATE_TOKEN])
  static getLogsCount(state: TestStateModel): number {
    return state.restQueryResponse.totalCount;
  }

  @Selector([TEST_STATE_TOKEN])
  static getCurrentPage(state: TestStateModel): number {
    return state.restQuery.currentPage.pageIndex;
  }

  @Selector([TEST_STATE_TOKEN])
  static getPageSize(state: TestStateModel): number {
    return state.restQuery.currentPage.pageSize;
  }

  @Action(Load)
  loadLogs(ctx: StateContext<TestStateModel>, _: Load) {
    return this.logsService.load(ctx.getState().restQuery.currentPage).pipe(
      tap(response => {
        const customResponse = new RestQueryResponse<TestModel[]>();
        customResponse.result = response.logs;
        customResponse.totalCount = response.logCount;

        ctx.patchState({
          restQueryResponse: customResponse,
        });
      }),
      catchError(error => {
        return throwError(error);
      })
    );
  }

  @Action(ChangePage)
  changePage(ctx: StateContext<TestStateModel>, action: ChangePage) {
    const customQuery = new RestQueryVo();
    customQuery.currentPage = action.event;

    ctx.patchState({
      restQuery: customQuery,
    });

    return ctx.dispatch(new Load());
  }
}
