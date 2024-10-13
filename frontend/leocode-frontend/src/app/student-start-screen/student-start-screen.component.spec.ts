import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentStartScreenComponent } from './student-start-screen.component';

describe('StudentStartScreenComponent', () => {
  let component: StudentStartScreenComponent;
  let fixture: ComponentFixture<StudentStartScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentStartScreenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentStartScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
