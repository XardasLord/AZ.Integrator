import { SourceSystem } from '../auth/models/source-system.model';

const prefix = '[Source System]';

export class ChangeSourceSystem {
  static readonly type = `${prefix} ${ChangeSourceSystem.name}`;

  constructor(public sourceSystem: SourceSystem) {}
}
