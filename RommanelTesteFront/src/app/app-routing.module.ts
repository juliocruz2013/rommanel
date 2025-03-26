import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { loginComponent } from './pages/login/login/login.component';
import { cadastrarComponent } from './pages/login/cadastrar/cadastrar.component';
import { clienteComponent } from './pages/cliente/cliente.component';
import { headerComponent } from './pages/header/header.component';
import { footerComponent } from './pages/footer/footer.component';
import { AutenticacaoGuard } from './common/autenticacao/autenticacao.guard';

const routes: Routes = [
  { path: 'login', component: loginComponent },
  { path: 'cadastrar', component: cadastrarComponent },
  { path: 'cliente', component: clienteComponent, canActivate: [AutenticacaoGuard] },
  { path: 'header', component: headerComponent, canActivate: [AutenticacaoGuard] },
  { path: 'footer', component: footerComponent, canActivate: [AutenticacaoGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AutenticacaoGuard],
})
export class AppRoutingModule {}
