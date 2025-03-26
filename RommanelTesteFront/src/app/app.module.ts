import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AutenticacaoGuard } from './common/autenticacao/autenticacao.guard';
import { RouterModule, Routes } from '@angular/router';
import { loginComponent } from './pages/login/login/login.component';
import { cadastrarComponent } from './pages/login/cadastrar/cadastrar.component';
import { clienteComponent } from './pages/cliente/cliente.component';
import { headerComponent } from './pages/header/header.component';
import { footerComponent } from './pages/footer/footer.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AutenticacaoInterceptor } from './common/autenticacao/autenticacao.interceptor';
import { DatePipe } from '@angular/common';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: loginComponent },
  { path: 'cadastrar', component: cadastrarComponent },
  { path: 'cliente', component: clienteComponent, canActivate: [AutenticacaoGuard] },
  { path: 'header', component: headerComponent, canActivate: [AutenticacaoGuard] },
  { path: 'footer', component: footerComponent, canActivate: [AutenticacaoGuard] },
];

@NgModule({
  declarations: [
    AppComponent,
    loginComponent,
    cadastrarComponent,
    clienteComponent,
    headerComponent,
    footerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
  ],

  providers: [
    AutenticacaoGuard,
    DatePipe,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AutenticacaoInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
