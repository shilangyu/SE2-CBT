export class ApiClient {
    constructor(private baseUrl: string) {
        console.log(`Initialized ApiClient for ${baseUrl}`)

        fetch(`${baseUrl}/WeatherForecast`).then(console.log)
    }
}
