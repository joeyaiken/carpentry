import React from 'react'
import {TrackedSets} from "./TrackedSets";
import {CollectionTotals} from "./CollectionTotals";
import {AppLayout} from "../common/components/AppLayout";
import {useAppSelector} from "../../hooks";
import {ApiStatus} from "../../enums";

export const Settings = (): JSX.Element => {
  const isLoading = useAppSelector(state =>
    state.settings.collectionTotals.status === ApiStatus.loading
    || state.settings.trackedSets.readDataStatus === ApiStatus.loading
  );
  
  return (
    <AppLayout title="Settings" isLoading={isLoading}>
      <CollectionTotals />
      <TrackedSets />
    </AppLayout>
  )
}