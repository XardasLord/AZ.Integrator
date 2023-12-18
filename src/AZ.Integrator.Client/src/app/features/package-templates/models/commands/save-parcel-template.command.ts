import { ParcelTemplate } from '../parcel-template';

export interface SaveParcelTemplateCommand {
  tag: string;
  parcelTemplates: ParcelTemplate[];
}
