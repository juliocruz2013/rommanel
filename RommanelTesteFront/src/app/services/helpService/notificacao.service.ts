import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GlobalConstants } from 'src/app/common/global-constants';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class NotificacaoService {
  readonly apiURL = GlobalConstants.apiURL;
  readonly headersGet = GlobalConstants.headersGet;
  readonly headersPost = GlobalConstants.headersPost;

  constructor(private http: HttpClient) { }

  AlertaConcluidoAzul(Titulo: string, Texto: string, Botao: string) {
    return Swal.fire({
      title: Titulo,
      text: Texto,
      iconHtml: '<img src="/assets/imagens/notificacoes/h_ok_azul_redondo.svg" width="50">',
      confirmButtonText: Botao,
      showCloseButton: true,
    });
  }

  AlertaErroConfirmacao(
    Titulo: string,
    Texto: string,
    BotaoOk: string,
    BotaoCancelar: string
  ) {
    return Swal.fire({
      title: Titulo,
      text: Texto,
      iconHtml: '<img src="/assets/imagens/notificacoes/h_erro_redondo.svg" width="50">',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: BotaoOk,
      cancelButtonText: BotaoCancelar,
      showCloseButton: true,
    });
  }

  AlertaErro(Titulo: string, Texto: string, BotaoOk: string) {
    return Swal.fire({
      title: Titulo,
      text: Texto,
      iconHtml: '<img src="/assets/imagens/notificacoes/h_erro_redondo.svg" width="50">',
      showCancelButton: false,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: BotaoOk,
      showCloseButton: true,
    });
  }

  AlertaConfirmacaoExclusao(
    Titulo: string,
    Texto: string,
    BotaoOk: string,
    BotaoCancelar: string
  ) {
    return Swal.fire({
      title: Titulo,
      text: Texto,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: BotaoOk,
      cancelButtonText: BotaoCancelar,
    });
  }
}
