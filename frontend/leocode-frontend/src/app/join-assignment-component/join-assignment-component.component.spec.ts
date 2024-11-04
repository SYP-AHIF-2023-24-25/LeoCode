import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JoinAssignmentComponentComponent } from './join-assignment-component.component';

describe('JoinAssignmentComponentComponent', () => {
  let component: JoinAssignmentComponentComponent;
  let fixture: ComponentFixture<JoinAssignmentComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [JoinAssignmentComponentComponent]
    });
    fixture = TestBed.createComponent(JoinAssignmentComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
