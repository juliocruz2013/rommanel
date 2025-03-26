import { Component, Renderer2 } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { NotificacaoService } from 'src/app/services/helpService/notificacao.service';
import { ClienteService } from 'src/app/services/cliente/cliente.service';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cliente',
  templateUrl: './cliente.component.html',
  styleUrls: ['./cliente.component.css'],
})
export class clienteComponent {
  clienteForm: FormGroup;
  clientes: any[] = [];
  editing: boolean = false;
  clienteId: number | null = null;
  sortOrder: string = 'asc';
  sortColumn: string = '';
  termoDePesquisa = new Subject<string>();

  constructor(
    private formBuilder: FormBuilder,
    private clienteService: ClienteService,
    private notificacaoService: NotificacaoService,
    private renderer: Renderer2
  ) {
    this.clienteForm = this.formBuilder.group({
      id: [null],
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      birthDate: ['', Validators.required],
      phone: ['', Validators.required],
      isCompany: [false],
      isExempt: [false],
      documentNumber: ['', Validators.required],
      cpf: ['', Validators.required],
      cnpj: [''],
      stateRegistration: [''],
      address: this.formBuilder.group({
        zipCode: ['', Validators.required],
        street: ['', Validators.required],
        number: ['', Validators.required],
        neighborhood: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
      }),
    });
  }

  ngOnInit(): void {
    this.listarClientes();

    this.clienteForm.get('isCompany')?.valueChanges.subscribe((isCompany) => {
      const cpfControl = this.clienteForm.get('cpf');
      const cnpjControl = this.clienteForm.get('cnpj');

      if (isCompany) {
        cnpjControl?.setValidators([Validators.required]);
        cnpjControl?.updateValueAndValidity();

        cpfControl?.clearValidators();
        cpfControl?.updateValueAndValidity();

        this.clienteForm.get('isExempt')?.setValue(false);
        this.clienteForm.get('cpf')?.setValue('');
        this.clienteForm.get('documentNumber')?.setValue(cnpjControl?.value || '');
      } else {
        cpfControl?.setValidators([Validators.required]);
        cpfControl?.updateValueAndValidity();

        cnpjControl?.clearValidators();
        cnpjControl?.updateValueAndValidity();

        this.clienteForm.get('cnpj')?.setValue('');
        this.clienteForm.get('documentNumber')?.setValue(cpfControl?.value || '');
      }
    });

    this.clienteForm.get('isExempt')?.valueChanges.subscribe((isExempt) => {
      if (isExempt) {
        this.clienteForm.get('stateRegistration')?.setValue('');
      }
    });

    this.clienteForm.get('cpf')?.valueChanges.subscribe((cpf) => {
      if (!this.clienteForm.get('isCompany')?.value) {
        this.clienteForm.get('documentNumber')?.setValue(cpf);
      }
    });

    this.clienteForm.get('cnpj')?.valueChanges.subscribe((cnpj) => {
      if (this.clienteForm.get('isCompany')?.value) {
        this.clienteForm.get('documentNumber')?.setValue(cnpj);
      }
    });
  }


  cpfValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const cpf = control.value?.replace(/[\D]/g, '');
    if (!cpf || cpf.length !== 11) return { invalidCpf: true };

    let sum = 0;
    let rest;
    if (cpf === '00000000000') return { invalidCpf: true };

    for (let i = 1; i <= 9; i++) sum += parseInt(cpf.charAt(i - 1)) * (11 - i);
    rest = (sum * 10) % 11;

    if (rest === 10 || rest === 11) rest = 0;
    if (rest !== parseInt(cpf.charAt(9))) return { invalidCpf: true };

    sum = 0;
    for (let i = 1; i <= 10; i++) sum += parseInt(cpf.charAt(i - 1)) * (12 - i);
    rest = (sum * 10) % 11;

    if (rest === 10 || rest === 11) rest = 0;
    if (rest !== parseInt(cpf.charAt(10))) return { invalidCpf: true };

    return null;
  }

  salvarCliente(event: Event): void {
    event.preventDefault();
    this.clienteForm.markAllAsTouched();
    debugger;
    if (this.clienteForm.valid) {
      const clienteModel = this.clienteForm.value;

      if (!clienteModel.id) {
        clienteModel.id = 0;
        this.clienteService.cadastrarCliente(clienteModel).subscribe(
          () => {
            this.scrollToTop();
            this.notificacaoService.AlertaConcluidoAzul('Sucesso', 'Cliente cadastrado com sucesso!', 'Concluir').then(() => {
              this.clienteForm.reset();
              this.listarClientes();
            });
          },
          (error) => {
            this.notificacaoService.AlertaErro('Ops!', error.error.message === 'Validation error' ? error.error.errors[0].value : error.error.message, 'Concluir').then(() => {
              this.listarClientes();
            });
          }
        );
      } else {
        this.updateCliente();
      }
    } else {
      this.notificacaoService
        .AlertaErro('Erro', 'Por favor, preencha todos os campos obrigatórios corretamente.', 'Concluir')
        .then(() => {
          this.clienteId = null;
        });
    }
  }

  listarClientes(): void {
    this.clienteService.listarClientes().subscribe(
      (response) => {
        this.clientes = response;
      },
      (error) => {
        this.notificacaoService.AlertaErro(
          'Erro ao tentar buscar a lista de Clientes',
          error.error.message,
          'Concluir'
        );
      }
    );
  }

  listarClientePorId(id: number): void {
    this.clienteService.listarClientePorId(id).subscribe(
      (response) => {
        this.clienteForm.patchValue({
          id: response.id,
          name: response.name,
          documentNumber: response.documentNumber,
          email: response.email,
          birthDate: this.formatarData(response.birthDate),
          phone: response.phone,
          isCompany: response.isCompany,
          stateRegistration: response.stateRegistration,
          isExempt: response.isExempt,
          cpf: !response.isCompany ? response.documentNumber : '',
          cnpj: response.isCompany ? response.documentNumber : '',
          address: {
            zipCode: response.address?.zipCode,
            street: response.address?.street,
            number: response.address?.number,
            neighborhood: response.address?.neighborhood,
            city: response.address?.city,
            state: response.address?.state,
          },
        });
      },
      () => {
        this.notificacaoService.AlertaErro('Erro', 'Erro ao listar os Clientes!', 'Concluir');
      }
    );
  }

  editarCliente(id: number): void {
    this.scrollToTop();
    this.listarClientePorId(id);
  }

  updateCliente(): void {
    if (this.clienteForm.valid) {
      const clienteModel = this.clienteForm.value;
      this.clienteService.editarCliente(clienteModel).subscribe(
        () => {
          this.editing = false;
          this.clienteId = null;
          this.clienteForm.reset();
          this.listarClientes();
        },
        (error) => {
          this.notificacaoService.AlertaErro('Erro', error.error.message, 'Concluir');
        }
      );
    }
  }

  excluirCliente(id: number) {
    this.clienteService.excluirCliente(id).subscribe(
      () => {
        Swal.fire('Excluído!', 'Cliente excluído.', 'success');
        this.listarClientes();
        this.clienteForm.reset();
      },
      (error) => {
        this.notificacaoService.AlertaErro('Erro', 'Erro ao excluir o cliente!', 'Concluir');
      }
    );
  }

  private formatarData(data: string): string {
    if (!data) return '';
    const [dia, mes, ano] = data.split('/');
    return `${ano}-${mes.padStart(2, '0')}-${dia.padStart(2, '0')}`;
  }

  resetForm(): void {
    this.clienteForm.reset();
    this.editing = false;
    this.clienteId = null;
  }

  scrollToTop(): void {
    this.renderer.setProperty(document.body, 'scrollTop', 0);
    this.renderer.setProperty(document.documentElement, 'scrollTop', 0);
  }

  scrollToBottom(): void {
    this.renderer.setProperty(document.body, 'scrollTop', document.body.scrollHeight);
    this.renderer.setProperty(document.documentElement, 'scrollTop', document.documentElement.scrollHeight);
  }

  sort(column: string): void {
    if (column === this.sortColumn) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortOrder = 'asc';
    }

    this.clientes.sort((a, b) => {
      const aValue = a[column];
      const bValue = b[column];
      return this.sortOrder === 'asc' ? (aValue > bValue ? 1 : -1) : (bValue > aValue ? 1 : -1);
    });
  }
}
