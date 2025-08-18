import { SaveParcelTemplateCommand } from '../models/commands/save-parcel-template.command';
import { PageEvent } from '@angular/material/paginator';

const prefix = '[Package Templates]';

export class LoadTemplates {
  static readonly type = `${prefix} ${LoadTemplates.name}`;
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;

  constructor(public event: PageEvent) {}
}

export class ApplyFilter {
  static readonly type = `${prefix} ${ApplyFilter.name}`;

  constructor(public searchPhrase: string) {}
}

export class SavePackageTemplate {
  static readonly type = `${prefix} ${SavePackageTemplate.name}`;

  constructor(public command: SaveParcelTemplateCommand) {}
}
