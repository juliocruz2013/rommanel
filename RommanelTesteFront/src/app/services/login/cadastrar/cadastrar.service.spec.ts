import { TestBed } from '@angular/core/testing';

import { cadastrarService } from './cadastrar.service';

describe('cadastrarService', () => {
  let service: cadastrarService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(cadastrarService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
