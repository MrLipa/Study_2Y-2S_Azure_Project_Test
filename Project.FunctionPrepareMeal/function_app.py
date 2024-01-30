import azure.functions as func
import logging
from MealOptimizer import MealOptimizer
import json

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.route(route="optimize_meal", methods=['POST'])
def http_trigger(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    try:
        data = req.get_json()
    except ValueError:
        return func.HttpResponse("Invalid JSON", status_code=400)
    
    print(data)

    products = data.get('products')
    limits = data.get('limits')

    if not products or not limits:
        return func.httpresponse(
            "please provide both products and limits in the json payload.",
            status_code=400
        )
    
    optimizer = MealOptimizer(products, limits)
    optimized_products = optimizer.optimize_meal()

    return func.HttpResponse(json.dumps(optimized_products), status_code=200, mimetype="application/json")