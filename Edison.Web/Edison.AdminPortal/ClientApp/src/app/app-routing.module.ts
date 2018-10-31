import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { AuthGuardService } from './core/services/auth-guard.service'

const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: 'configuration',
    loadChildren:
      './modules/configuration/configuration.module#ConfigurationModule',
    canLoad: [AuthGuardService],
    data: {
      title: 'CONFIGURATION',
    },
  },
  {
    path: 'dashboard',
    loadChildren: './modules/dashboard/dashboard.module#DashboardModule',
    canLoad: [AuthGuardService],
    data: {
      title: 'RIGHT NOW',
    },
  },
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
