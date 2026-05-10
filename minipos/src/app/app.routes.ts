import { Routes } from '@angular/router';
import { NotFound } from './not-found/not-found';
import { authGuard } from './core/auth/auth.guard';
import { Unauthorized } from './unauthorized/unauthorized';
import { Register } from './features/register/register';
import { Login } from './features/login/login';
import { Dashboard } from './features/dashboard/dashboard';
import { Home } from './home/home';

export const routes: Routes = [
    {path:'',component:Dashboard},
    {path:'home',component:Dashboard},
    {path:'dashboard',component:Dashboard, canActivate: [authGuard]},
    {path:'login',component:Login},
    {path:'register',component:Register},
    {path:'unauthorized',component:Unauthorized},
    //{path:'job/:id',component:JobDetails, canActivate: [authGuard]},
    {path:'**',component:NotFound}

];
