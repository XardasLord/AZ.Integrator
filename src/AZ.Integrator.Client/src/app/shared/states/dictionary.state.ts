import { Injectable, inject } from '@angular/core';
import { State, StateToken } from '@ngxs/store';
import { DictionaryStateModel } from './dictionary.state.model';
import { DictionaryService } from '../services/dictionary.service';

export const DICTIONARY_STATE_TOKEN = new StateToken<DictionaryStateModel>('dictionary');

@State<DictionaryStateModel>({
  name: DICTIONARY_STATE_TOKEN,
  defaults: {
    systemGroups: [],
    systemRoles: [],
    systemRegions: [],
    systemPermissions: [],
    mapObjectTypes: [],
    mapDeviceTypes: [],
    mapObjectStatusTypes: [],
    deviceGroupsRelation: [],
    timeExtentDefinitions: [],
    configDefinitions: [],
  },
})
@Injectable()
export class DictionaryState {  private dictionaryService = inject(DictionaryService);

}
