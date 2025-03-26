import { ComponentFixture, TestBed } from '@angular/core/testing';

import { cadastrarComponent } from './cadastrar.component.js';

describe('cadastrarComponent', () => {
  let component: cadastrarComponent;
  let fixture: ComponentFixture<cadastrarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [cadastrarComponent],
    });
    fixture = TestBed.createComponent(cadastrarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
