import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignmentOverviewComponent } from './assignment-overview.component';

describe('AssignmentOverviewComponent', () => {
  let component: AssignmentOverviewComponent;
  let fixture: ComponentFixture<AssignmentOverviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AssignmentOverviewComponent]
    });
    fixture = TestBed.createComponent(AssignmentOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
