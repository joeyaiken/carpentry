import React from 'react'
// import {
//   Box,
//   Typography,
//   Button,
//   Switch,
//   FormControlLabel,
//   IconButton,
//   TableHead,
//   Paper,
//   Table,
//   TableRow,
//   TableCell,
//   TableBody,
// } from '@material-ui/core';
// import { Refresh, Add, Delete } from '@material-ui/icons';
// import { appStyles } from '../../styles/appStyles';
// import AppLayout from '../../common/components/AppLayout';
//
// declare interface ComponentProps {
//   onRefreshClick: () => void;
//   onShowUntrackedClick: () => void;
//
//   onAddSetClick: (setId: number) => void;
//   onRemoveSetClick: (setId: number) => void;
//   onUpdateSetClick: (setId: number) => void;
//
//   trackedSetDetails: SetDetailDto[];
//   showUntrackedValue: boolean;
//
//   isLoading: boolean;
// }
//
// export default function TrackedSetsContainerLayout(props: ComponentProps): JSX.Element {
//   const { flexRow, flexSection } = appStyles();
//   return(
//     <AppLayout title="Settings - Tracked Sets" isLoading={props.isLoading}>
//       {/*<Backdrop open={props.isLoading} style={{zIndex:0}} >*/}
//       {/*  <CircularProgress color="inherit" />*/}
//       {/*</Backdrop>*/}
//       <Box>
//         <Box className={flexRow}>
//           <Box className={flexSection}>
//             <Typography variant="h4">
//               Tracked Sets
//             </Typography>
//           </Box>
//           <FormControlLabel id='show-untracked-toggle'
//             onClick={props.onShowUntrackedClick}
//             control={
//               <Switch
//                 checked={props.showUntrackedValue}
//                 name="checkedB"
//                 color="primary" />
//             }
//             label="Show Untracked"
//           />
//           <Button disabled={true} color="primary" variant="contained" >Update All</Button>
//           <IconButton color="inherit" onClick={props.onRefreshClick} id="refresh-button" >
//             <Refresh />
//           </IconButton>
//         </Box>
//         <Paper>
//           <Table size="small">
//             <TableHead>
//               <TableRow>
//                 <TableCell>Code</TableCell>
//                 <TableCell>Name</TableCell>
//                 <TableCell>Owned</TableCell>
//                 <TableCell>Collected</TableCell>
//                 <TableCell>Last Updated</TableCell>
//                 <TableCell>Actions</TableCell>
//               </TableRow>
//             </TableHead>
//             <TableBody>
//               { props.trackedSetDetails.map(setDetail =>
//                 <TableRow key={setDetail.code} className="set-row">
//                   <TableCell>{setDetail.code}</TableCell>
//                   <TableCell>{setDetail.name}</TableCell>
//                   <TableCell>{setDetail.inventoryCount}</TableCell>
//                   <TableCell>{setDetail.isTracked && `${setDetail.collectedCount}/${setDetail.totalCount}`}</TableCell>
//                   <TableCell>{setDetail.dataLastUpdated}</TableCell>
//                   <TableCell>
//                     { !setDetail.isTracked &&
//                       <IconButton className="add-button" color="inherit" onClick={() => {props.onAddSetClick(setDetail.setId)}}>
//                         <Add />
//                       </IconButton>
//                     }
//                     { setDetail.isTracked &&
//                       <React.Fragment>
//                         <IconButton color="inherit" onClick={() => {props.onUpdateSetClick(setDetail.setId)}}>
//                           <Refresh />
//                         </IconButton>
//                         <IconButton color="inherit" onClick={() => {props.onRemoveSetClick(setDetail.setId)}}>
//                           <Delete />
//                         </IconButton>
//                       </React.Fragment>
//                     }
//                   </TableCell>
//                 </TableRow>
//               )}
//             </TableBody>
//           </Table>
//         </Paper>
//       </Box>
//     </AppLayout>
//   );
// }