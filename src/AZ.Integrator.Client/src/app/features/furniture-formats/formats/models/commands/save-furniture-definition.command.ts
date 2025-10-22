import { PartDefinitionDto } from '../part-definition.dto';

export interface SaveFurnitureDefinitionCommand {
  furnitureCode: string;
  partDefinitions: PartDefinitionDto[];
}
