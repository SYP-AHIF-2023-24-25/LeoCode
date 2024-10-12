import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApiDemoComponent } from './api-demo.component';

describe('ApiDemoComponent', () => {
  let component: ApiDemoComponent;
  let fixture: ComponentFixture<ApiDemoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ApiDemoComponent]
    });
    fixture = TestBed.createComponent(ApiDemoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
