import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { DashboardComponent } from './pages/dashboard/dashboard.component'
import { EventBarComponent } from './components/event-bar/event-bar.component'
import { RecentlyActiveComponent } from '../messaging/components/recently-active/recently-active.component'

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: '',
        component: EventBarComponent,
        data: {
          title: 'RIGHT NOW',
        },
      },
      {
        path: 'messaging',
        component: RecentlyActiveComponent,
        data: {
          title: 'MESSAGING',
        },
      },
    ],
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DashboardRoutingModule {}
