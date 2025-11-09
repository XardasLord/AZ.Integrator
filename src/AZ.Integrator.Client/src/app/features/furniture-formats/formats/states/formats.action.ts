import { PageEvent } from '@angular/material/paginator';
import { SaveFurnitureDefinitionCommand } from '../models/commands/save-furniture-definition.command';

const prefix = '[Furniture Formats]';

export class LoadFurnitureDefinitions {
  static readonly type = `${prefix} ${LoadFurnitureDefinitions.name}`;
}

export class LoadAllFurnitureDefinitions {
  static readonly type = `${prefix} ${LoadAllFurnitureDefinitions.name}`;
}

export class ChangePage {
  static readonly type = `${prefix} ${ChangePage.name}`;

  constructor(public event: PageEvent) {}
}

export class ApplyFilter {
  static readonly type = `${prefix} ${ApplyFilter.name}`;

  constructor(public searchPhrase: string) {}
}

export class AddFurnitureDefinition {
  static readonly type = `${prefix} ${AddFurnitureDefinition.name}`;

  constructor(public command: SaveFurnitureDefinitionCommand) {}
}

export class UpdateFurnitureDefinition {
  static readonly type = `${prefix} ${UpdateFurnitureDefinition.name}`;

  constructor(public command: SaveFurnitureDefinitionCommand) {}
}

export class DeleteFurnitureDefinition {
  static readonly type = `${prefix} ${DeleteFurnitureDefinition.name}`;

  constructor(public furnitureCode: string) {}
}

export class OpenFurnitureDefinitionDialog {
  static readonly type = `${prefix} ${OpenFurnitureDefinitionDialog.name}`;

  constructor(public furnitureCode?: string) {}
}
