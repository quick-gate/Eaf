import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EafCoreComponent } from './eaf-core.component';

describe('EafCoreComponent', () => {
  let component: EafCoreComponent;
  let fixture: ComponentFixture<EafCoreComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EafCoreComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EafCoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
