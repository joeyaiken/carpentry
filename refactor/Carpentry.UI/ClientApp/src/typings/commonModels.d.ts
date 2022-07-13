/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

// (IDK if these commented lines actually need to exist or not)

declare interface NormalizedList<T> {
  byId: { [key:number]: T };
  allIds: number[];
}



