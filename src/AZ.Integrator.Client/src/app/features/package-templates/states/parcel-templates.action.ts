import { SaveParcelTemplateCommand } from '../models/commands/save-parcel-template.command';

const prefix = '[Package Templates]';

export class LoadProductTags {
  static readonly type = `${prefix} ${LoadProductTags.name}`;
}

export class OpenPackageTemplateDefinitionModal {
  static readonly type = `${prefix} ${OpenPackageTemplateDefinitionModal.name}`;

  constructor(public tag: string) {}
}

export class SavePackageTemplate {
  static readonly type = `${prefix} ${SavePackageTemplate.name}`;

  constructor(public command: SaveParcelTemplateCommand) {}
}
