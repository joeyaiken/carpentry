import React from 'react';
import {Box} from "@material-ui/core";
import InventoryCardGrid from "./components/InventoryCardGrid";
import {ApiStatus} from "../../../enums";
import {useAppSelector} from "../../../hooks";
import {AppLayout} from "../../common/components/AppLayout";
import LoadingBox from "../../../common/components/LoadingBox";
import {InventoryFilterAppBar} from "./components/InventoryFilterAppBar";

export const InventoryOverview = (): JSX.Element => {
  const isLoading = useAppSelector(state => state.inventory.overviews.data.status === ApiStatus.loading);
  return (
    <AppLayout title="Inventory" isLoading={isLoading}>
      <Box >
        <InventoryFilterAppBar />
        <Box>
          { (isLoading) ? <LoadingBox /> : <InventoryCardGrid /> }
        </Box>
      </Box>
    </AppLayout>
  );
}

