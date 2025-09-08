import http from 'k6/http';
import { sleep, check } from 'k6';

export let options = {
    stages: [
        { duration: '10s', target: 10 }, // ramp up to 10 virtual users
        { duration: '30s', target: 10 }, // stay at 10 VUs
        { duration: '10s', target: 0 },  // ramp down
    ],
    thresholds: {
        http_req_duration: ['p(95)<7000'], // 95% of requests should be below 7000ms at high loads
    },
};

export default function () {
    const url = 'https://localhost:7075/HTMLtoPDF/generate/pdf';
    
    const payload = JSON.stringify({
        sdk: "puppeteer",
        title: "FinOps PDF Report",
        includeLineCharts: true,
        includeBarCharts: true,
        includeStackedCharts: true,
        includeMixedCharts: true,
        includePieCharts: true,
        includeDonutCharts: true,
        includeRadarCharts: true,
        includeAreaCharts: true
    });

    const params = {
        headers: {
            'Content-Type': 'application/json'
        },
    };

    const res = http.post(url, payload, params);

    // validate response
    check(res, {
        'status is 200': (r) => r.status === 200,
        'body not empty': (r) => r.body.length > 0,
    });

    sleep(0.5);
}
