import { makeStyles } from '@material-ui/core';

export const appStyles = makeStyles({
    flexRow: {
        display: "flex",
        flexFlow: "row",
    },
    flexRowWrap: {
        display: "flex",
        flexFlow: "row",
        flexWrap: "wrap",
    },
    flexCol: {
        display: "flex",
        flexFlow: "column",
    },
    flexContainerVertical: {
        display: "flex",
        flexFlow: "column",
    },
    flexSection: {
        flex: '1 1 0%'
    },
    staticSection: {
        flex: "0 0 0%",
    },
    scrollSection: {
        flex: "1 1 0%",
        overflow: "auto",
    },
    outlineSection: {
        border: "solid #9E9E9E 1px",
        padding: "8px",
        margin: "8px",
        backgroundColor: "#FFFFFF",
    },
    stretch:{
        width: '100%',
        height: '100%',
    }
});