<!DOCTYPE html>
<html>
<head>
    <title>Advanced Trading Strategy Generator with Optimization</title>
    <style>
        body { max-width: 1200px; margin: 20px auto; padding: 20px; font-family: 'Segoe UI', Arial, sans-serif; background: #f0f2f5; }
        .container { background: white; padding: 30px; border-radius: 12px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }
        .controls { display: flex; gap: 10px; margin-bottom: 20px; align-items: center; flex-wrap: wrap; }
        button { padding: 12px 24px; cursor: pointer; background: #2563eb; color: white; border: none; border-radius: 6px; transition: background 0.3s ease; }
        button:hover { background: #1d4ed8; }
        textarea { padding: 12px; border: 2px solid #e5e7eb; border-radius: 6px; width: 400px; height: 100px; resize: vertical; }
        textarea:focus { border-color: #2563eb; outline: none; }
        pre { background: #1e293b; color: #e2e8f0; padding: 20px; border-radius: 8px; white-space: pre-wrap; overflow-x: auto; }
        .optimization-status { padding: 15px; margin-bottom: 20px; background: #dbeafe; border-radius: 6px; border-left: 4px solid #2563eb; min-height: 20px; }
    </style>
</head>
<body>
    <div class="container">
        <h2>Trading Strategy Generator with Performance Optimization</h2>
        <div class="optimization-status" id="optimizationStatus">Ready to generate new values</div>
        <div class="controls">
            <button onclick="generateScript()">Generate New Values</button>
            <button onclick="copyScript()">Copy Script</button>
            <button onclick="exportCurrentBest()">Export Current Best</button>
            <button onclick="resetOptimization()">Reset Optimization</button>
            <textarea id="performanceMetrics" placeholder="Paste performance metrics here"></textarea>
            <button onclick="saveAndOptimize()">Save & Update Optimization</button>
        </div>
        <pre id="scriptOutput"></pre>
    </div>

<script>
const defaultRanges = {
    LongEntryFastEMA: [5, 70],
    LongEntrySlowEMA: [100, 250],
    LongExitFastEMA: [100, 250],
    LongExitSlowEMA: [2, 70],
    ShortEntryFastEMA: [2, 70],
    ShortEntrySlowEMA: [110, 250],
    ShortExitFastEMA: [100, 200],
    ShortExitSlowEMA: [2, 500],
    ADX_Length: [4, 100],
    Stoch1_Length: [5, 200],
    Stoch2_Length: [5, 200],
    Stoch14_Threshold: [30, 60],
    Stoch28_Threshold: [5, 40],
    LongADX_Threshold: [1, 20],
    ShortADX_Threshold: [1, 20]
};

let bestPerformers = {
    profit: 0,
    carMdd: 0,
    settings: null
};

const savedOptimization = localStorage.getItem('bestPerformers');
if (savedOptimization) {
    bestPerformers = JSON.parse(savedOptimization);
}

function getRandomInt(min, max, step = 1) {
    const steps = Math.floor((max - min) / step);
    return min + (Math.floor(Math.random() * (steps + 1)) * step);
}

function getOptimizedValue(min, max, bestValue, step = 1) {
    const range = bestValue * 0.15;
    return getRandomInt(
        Math.max(min, Math.floor(bestValue - range)),
        Math.min(max, Math.ceil(bestValue + range)),
        step
    );
}

function generateValues() {
    const useOptimized = bestPerformers.profit > 0 && Math.random() < 0.5 && bestPerformers.settings;
    const values = {};
    
    for (let key in defaultRanges) {
        let step = 1;
        if (key.includes('EMA')) step = 5;
        if (key === 'ADX_Length') step = 2;
        if (key.includes('Stoch') && key.includes('Length')) step = 5;
        
        let value;
        if (useOptimized && bestPerformers.settings && bestPerformers.settings[key]) {
            value = getOptimizedValue(
                defaultRanges[key][0],
                defaultRanges[key][1],
                bestPerformers.settings[key],
                step
            );
        } else {
            value = getRandomInt(
                defaultRanges[key][0],
                defaultRanges[key][1],
                step
            );
        }
        
        values[key] = Math.max(
            defaultRanges[key][0], 
            Math.min(defaultRanges[key][1], value)
        );
    }
    
    document.getElementById('optimizationStatus').textContent = useOptimized ? 
        `Optimization Mode - Using values within 2% of best profit: ${bestPerformers.profit.toFixed(2)} | CAR/MDD: ${bestPerformers.carMdd.toFixed(2)}` : 
        'Exploration Mode - Using full range values';
    
    return values;
}

function generateScript() {
    const values = generateValues();
    const script = `_SECTION_BEGIN("Double EMA Cross ADX Stoch Strategy");

// Time Rules
TimeOK = (TimeNum() >= 093000) AND (TimeNum() <= 154500);
ForcedExit = (TimeNum() >= 155500);

// LONG Strategy Parameters - EMA values
LongEntryFastEMA = ${values.LongEntryFastEMA};
LongEntrySlowEMA = ${values.LongEntrySlowEMA};
LongExitFastEMA = ${values.LongExitFastEMA};
LongExitSlowEMA = ${values.LongExitSlowEMA};

// SHORT Strategy Parameters - EMA values
ShortEntryFastEMA = ${values.ShortEntryFastEMA};
ShortEntrySlowEMA = ${values.ShortEntrySlowEMA};
ShortExitFastEMA = ${values.ShortExitFastEMA};
ShortExitSlowEMA = ${values.ShortExitSlowEMA};

// Stochastic Parameters
Stoch14_Threshold = Optimize("Stoch14 Threshold", 20, 15, 70, 55);
Stoch28_Threshold = Optimize("Stoch28 Threshold", 20, 15, 70, 55);

// Calculate EMAs for LONG
LongEntryFastLine = EMA(C, LongEntryFastEMA);
LongEntrySlowLine = EMA(C, LongEntrySlowEMA);
LongExitFastLine = EMA(C, LongExitFastEMA);
LongExitSlowLine = EMA(C, LongExitSlowEMA);

// Calculate EMAs for SHORT
ShortEntryFastLine = EMA(C, ShortEntryFastEMA);
ShortEntrySlowLine = EMA(C, ShortEntrySlowEMA);
ShortExitFastLine = EMA(C, ShortExitFastEMA);
ShortExitSlowLine = EMA(C, ShortExitSlowEMA);

// ADX Parameters
ADX_Value = ADX(${values.ADX_Length});
LongADX_Threshold = Optimize("Long ADX Threshold", 25, 5, 95, 45);
ShortADX_Threshold = Optimize("Short ADX Threshold", 25, 2, 16, 4);

// Stochastic Calculations
Stoch14 = StochD(${values.Stoch1_Length});
Stoch28 = StochD(${values.Stoch2_Length});

// Entry Conditions
LongStochCondition = Stoch14 > Stoch14_Threshold AND Stoch28 > Stoch28_Threshold;
ShortStochCondition = Stoch14 < (100 - Stoch14_Threshold) AND Stoch28 < (100 - Stoch28_Threshold);

// Entry & Exit Signals
Buy = TimeOK AND Cross(LongEntryFastLine, LongEntrySlowLine) AND ADX_Value > LongADX_Threshold AND LongStochCondition;
Sell = Cross(LongExitSlowLine, LongExitFastLine) OR ForcedExit;

Short = TimeOK AND Cross(ShortEntrySlowLine, ShortEntryFastLine) AND ADX_Value > ShortADX_Threshold AND ShortStochCondition;
Cover = Cross(ShortExitFastLine, ShortExitSlowLine) OR ForcedExit;

SetPositionSize(1, spsShares);

_SECTION_END();`;

    document.getElementById('scriptOutput').textContent = script;
}

function parsePerformanceMetrics(text) {
    const lines = text.trim().split('\n');
    const dataLine = lines[lines.length - 1];
    const values = dataLine.split('\t');
    
    return {
        netProfit: parseFloat(values[1]),
        carMdd: parseFloat(values[11])
    };
}

function resetOptimization() {
    bestPerformers = {
        profit: 0,
        carMdd: 0,
        settings: null
    };
    localStorage.removeItem('bestPerformers');
    document.getElementById('optimizationStatus').textContent = 'Optimization reset - all best values cleared';
}

function saveAndOptimize() {
    const metricsText = document.getElementById('performanceMetrics').value;
    if (!metricsText) {
        document.getElementById('optimizationStatus').textContent = 'Please paste performance metrics';
        return;
    }

    const metrics = parsePerformanceMetrics(metricsText);
    const scriptText = document.getElementById('scriptOutput').textContent;
    const MINIMUM_PROFIT_THRESHOLD = 300;
    
    if (!isNaN(metrics.netProfit)) {
        const settings = extractCurrentValues(scriptText);
        const fileData = {
            profit: metrics.netProfit,
            carMdd: metrics.carMdd,
            settings,
            timestamp: new Date().toISOString(),
            fullPineScript: scriptText
        };
        
        if (metrics.netProfit > MINIMUM_PROFIT_THRESHOLD && metrics.netProfit > (bestPerformers.profit || 0)) {
            bestPerformers = { ...fileData };
            localStorage.setItem('bestPerformers', JSON.stringify(bestPerformers));
            document.getElementById('optimizationStatus').textContent = 
                `New best profit achieved: ${metrics.netProfit.toFixed(2)} | CAR/MDD: ${metrics.carMdd.toFixed(2)}`;
        }
        
        const blob = new Blob([scriptText], { type: 'text/plain' });
        const a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = `Profit_${metrics.netProfit.toFixed(2)}_CARMDD_${metrics.carMdd.toFixed(2)}.txt`;
        a.click();
    }
}

function exportCurrentBest() {
    if (bestPerformers.profit > 0 && bestPerformers.settings) {
        const blob = new Blob([bestPerformers.fullPineScript], { type: 'text/plain' });
        const a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = `current_best_Profit_${bestPerformers.profit.toFixed(2)}_CARMDD_${bestPerformers.carMdd.toFixed(2)}.txt`;
        a.click();
        
        document.getElementById('optimizationStatus').textContent = 
            `Current best exported (Profit: ${bestPerformers.profit.toFixed(2)} | CAR/MDD: ${bestPerformers.carMdd.toFixed(2)})`;
    } else {
        document.getElementById('optimizationStatus').textContent = 
            'No qualifying best performers saved yet!';
    }
}

function copyScript() {
    const scriptText = document.getElementById('scriptOutput').textContent;
    navigator.clipboard.writeText(scriptText);
    document.getElementById('optimizationStatus').textContent = 'Script copied to clipboard!';
}

function extractCurrentValues(scriptText) {
    const values = {};
    const variableMap = {
        'ADX_Length': 'ADX\\(([0-9]+)\\)',
        'Stoch1_Length': 'StochD\\(([0-9]+)\\)',
        'Stoch2_Length': 'StochD\\(([0-9]+)\\)'
    };
    
    for (let key in defaultRanges) {
        let pattern = variableMap[key] || `${key} = ([0-9]+)`;
        const regex = new RegExp(pattern);
        const match = scriptText.match(regex);
        if (match) {
            values[key] = parseFloat(match[1]);
        } else {
            values[key] = defaultRanges[key][0];
        }
    }
    return values;
}

// Initialize on page load
window.onload = function() {
    generateScript();
    document.getElementById('optimizationStatus').textContent = 'Ready to generate new values';
};
</script>
</body>
</html>
