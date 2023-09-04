import { NgModule } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { SharedModule } from '../../shared/shared.module';
import { TestRoutingModule } from './test-routing.module';
import { TestComponent } from './components/test/test.component';
import { TestListComponent } from './components/test-list/test-list.component';
import { TestService } from './services/test.service';
import { TestState } from './states/test.state';

@NgModule({
  declarations: [TestComponent, TestListComponent],
  imports: [SharedModule, TestRoutingModule, NgxsModule.forFeature([TestState])],
  providers: [TestService],
})
export class TestModule {}
