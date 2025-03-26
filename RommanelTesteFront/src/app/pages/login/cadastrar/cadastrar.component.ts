import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { CadastrarService } from 'src/app/services/login/cadastrar/cadastrar.service';
import { NotificacaoService } from 'src/app/services/helpService/notificacao.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cadastrar',
  templateUrl: './cadastrar.component.html',
  styleUrls: ['./cadastrar.component.css'],
})
export class cadastrarComponent implements OnInit {
  cadastroForm: FormGroup;
  perfis: any[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private cadastrarService: CadastrarService,
    private notificacaoService: NotificacaoService,
    private router: Router,
  ) {
    this.cadastroForm = this.formBuilder.group({});
  }

  ngOnInit(): void {

    this.cadastroForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      passwordConfirmation: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.cadastroForm.valid) {
      const cadastroData = this.cadastroForm.value

      this.cadastrarService.registrarUsuario(cadastroData).subscribe(
        (response) => {
          this.notificacaoService
            .AlertaConcluidoAzul(
              'Sucesso',
              'Usuário cadastrado com sucesso!',
              'Concluir'
            )
            .then(() => {
              this.router.navigate(['/login']);
            });
        },
        (error) => {
          this.notificacaoService
            .AlertaErro('Erro', error.error.message, 'Concluir')
            .then(() => {
              window.location.reload();
            });
        }
      );
    } else {
      this.cadastroForm.markAllAsTouched();

      if (this.cadastroForm.get('email')?.hasError('invalidDomain')) {
        this.notificacaoService
          .AlertaErro('Erro', 'O email deve ser @telefonica.com', 'Concluir')
          .then(() => {
            window.location.reload();
          });
      } else {
        this.notificacaoService
          .AlertaErro('Erro', 'Por favor, corrija os erros no formulário!', 'Concluir')
          .then(() => {
            window.location.reload();
          });
      }
    }
  }
}
