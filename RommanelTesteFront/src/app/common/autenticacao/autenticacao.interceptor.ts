import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap, catchError } from 'rxjs/operators';
import { AuthService } from 'src/app/services/helpService/Auth.service';
import { DatePipe } from '@angular/common';
import { NotificacaoService } from 'src/app/services/helpService/notificacao.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class AutenticacaoInterceptor implements HttpInterceptor {
  href: string = '';
  formData = new FormData();
  QuantidadeErro: number = 0;
  Url: string = '';

  constructor(
    private router: Router,
    private authService: AuthService,
    private spinner: NgxSpinnerService,
    private notificacaoService: NotificacaoService,
    private datePipe: DatePipe
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const isLoginRequest = req.url.includes('/login');

    let request = req;

    if (this.authService.isAuthenticated() && !isLoginRequest) {
      request = req.clone({
        setHeaders: {
          Authorization: `Bearer ${this.authService.getToken()}`,
        },
      });
    }

    return next.handle(request).pipe(
      tap({
        next: () => {
          if (this.Url !== req.url) {
            this.QuantidadeErro = 0;
          }
        },
        error: (err: HttpErrorResponse) => {
          if (err.status === 401) {
            this.authService.clearToken();
            this.router.navigateByUrl('/Erro/SessaoExpirada');
            return;
          }

          if (err.status === 403) {
            this.notificacaoService.AlertaErro(
              'Não Autorizado!',
              'Você não tem permissão para acessar os dados desta página!',
              'Fechar'
            );
            return;
          }

          this.PutLogErro(err);
          this.QuantidadeErro++;
          this.Url = err.url ?? '';
        },
      }),
      catchError((err) => {
        this.spinner.hide();
        return throwError(() => err);
      })
    );
  }

  PutLogErro(erro: HttpErrorResponse) {
    const erroMsg = erro.error?.retorno ?? erro.message;
    const erroAtual = localStorage.getItem('ErroUsuario');
    const erroData = localStorage.getItem('ErroData');
    const dataAtual = String(this.datePipe.transform(new Date(), 'yyyyMMddHHmm'));

    if (erroAtual === erroMsg || erroData === dataAtual) {
      this.spinner.hide();
      return;
    }

    localStorage.setItem('ErroUsuario', erroMsg);
    localStorage.setItem('ErroData', dataAtual);

    this.formData = new FormData();
    this.formData.append('Status', erro.status.toString());
    this.formData.append('Erro', erroMsg);
    this.formData.append('Url', erro.url ?? 'sem url');
    this.spinner.hide();
  }
}
