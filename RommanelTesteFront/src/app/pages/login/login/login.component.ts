import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { GlobalConstants } from 'src/app/common/global-constants';
import { LoginModel } from 'src/app/models/login/login/login-model';
import { LoginService } from 'src/app/services/login/login/login.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NotificacaoService } from 'src/app/services/helpService/notificacao.service';
import { AuthService } from 'src/app/services/helpService/Auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class loginComponent {
  loginForm: FormGroup;
  readonly minutesToken = GlobalConstants.minutesToken;
  showSpinner: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    public loginService: LoginService,
    private notificacaoService: NotificacaoService,
    private spinner: NgxSpinnerService,
    private authService: AuthService
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  ngOnInit(): void { }

  submitLogin() {
    this.showSpinner = true;
    const dadosLogin = this.loginForm.getRawValue() as LoginModel;

    this.loginService.LoginUsuario(dadosLogin).subscribe(
      (data: any) => {
        this.authService.setToken(data.token);

        debugger;
        this.router.navigate(['/cliente']);
        this.showSpinner = false;
      },
      (error) => {
        this.spinner.hide();
        this.showSpinner = false;

        this.notificacaoService
          .AlertaErro('Erro', error.error.message, 'Concluir')
          .then(() => {
            window.location.reload();
          });
      }
    );
  }
}
