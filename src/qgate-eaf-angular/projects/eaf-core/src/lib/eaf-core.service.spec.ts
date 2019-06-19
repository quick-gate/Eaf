import { TestBed } from '@angular/core/testing';

import { EafCoreService } from './eaf-core.service';

describe('EafCoreService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EafCoreService = TestBed.get(EafCoreService);
    expect(service).toBeTruthy();
  });
});
