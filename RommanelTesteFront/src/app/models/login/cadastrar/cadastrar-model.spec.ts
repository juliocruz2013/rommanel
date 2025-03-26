import { CadastrarModel } from './cadastrar-model';

describe('CadastrarModel', () => {
  it('should create', () => {
    const model: CadastrarModel = {
      email: 'test@example.com',
      senha: 'password',
      confirmacaoSenha: 'password'
    };
    expect(model).toBeTruthy();
  });
});
