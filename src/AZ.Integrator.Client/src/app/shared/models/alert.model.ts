import { MapObjectStatusTypeEnum } from './map-object-status-type.enum';

export interface AlertModel {
  serviceName: string;
  status: MapObjectStatusTypeEnum;
  message: string;
  details: string;
  timestamp: Date;
}
