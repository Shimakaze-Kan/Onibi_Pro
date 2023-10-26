import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalManagementComponent } from './personel-management.component';

describe('PersonalManagementComponent', () => {
  let component: PersonalManagementComponent;
  let fixture: ComponentFixture<PersonalManagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PersonalManagementComponent],
    });
    fixture = TestBed.createComponent(PersonalManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
