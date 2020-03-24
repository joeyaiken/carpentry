//TODO Review and verify if actually used
import React from 'react';
// import QuickCardCount from './QuickCardCount'
// import QuickManaCurve from './QuickManaCurve'
// import QuickSpellType from './QuickSpellType'
export interface DeckQuickStatsProps {


}

export default function DeckQuickStats(props: DeckQuickStatsProps): JSX.Element {
    return(
        <div className="sd-deck-quick-stats card">
            <div className="card-header">
                <label>Quick Stats</label>
            </div>
            <div className="flex-container-horizontal">
                {/* <QuickCardCount />
                <QuickManaCurve />
                <QuickSpellType /> */}
            </div>
        </div>
    );
}