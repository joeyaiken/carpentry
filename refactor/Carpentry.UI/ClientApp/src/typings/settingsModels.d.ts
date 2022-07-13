/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

// (IDK if these commented lines actually need to exist or not)

declare interface TrackedSetDto {
  setId: number;
  code: string;
  name: string;
  
  // ownedCount: number;
  // collectedCount: number;
  // totalCount: number;
  // isTracked: boolean;
  
  canBeAdded: boolean;
  canBeUpdated: boolean;
  canBeRemoved: boolean;
  
  lastUpdated: string;
  // canBeUpdated: boolean;
}