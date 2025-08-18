import { ParcelTemplate } from '../../models/parcel-template';

export interface PackageTemplateDefinitionFormDialogResponseModel {
  tag: string;
  parcels: ParcelTemplate[];
}
