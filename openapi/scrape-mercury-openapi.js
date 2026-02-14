#!/usr/bin/env node

/**
 * Scrape Mercury API .md endpoints to build complete OpenAPI spec
 * 
 * Usage: node scrape-mercury-openapi.js
 */

const https = require('https');
const fs = require('fs');

// Base endpoints from the screenshot
const endpoints = [
  // Accounts
  'getaccount',
  'getaccountcards',
  'requestsendmoney',
  'getaccountstatements',
  'gettransaction',
  'listaccounttransactions',
  'createtransaction',
  'getaccounts',
  'createinternaltransfer',
  
  // Recipients
  'getrecipient',
  'getrecipients',
  'updaterecipient',
  'createrecipient',
  'uploadrecipientattachment',
  'listrecipientsattachments',
  
  // Accounts Receivable
  'getattachment',
  'listinvoices',
  'createinvoice',
  'getinvoice',
  'updateinvoice',
  'cancelinvoice',
  'listinvoiceattachments',
  'getinvoicepdf',
  'listcustomers',
  'createcustomer',
  'deletecustomer',
  'getcustomer',
  'updatecustomer',
  
  // Treasury
  'gettreasury',
  'gettreasurytransactions',
  
  // Statements
  'getstatementpdf',
  
  // Transactions
  'updatetransaction',
  'listtransactions',
  'gettransactionbyid',
  'uploadtransactionattachment',
  
  // Categories
  'listcategories',
  
  // Credit
  'listcredit',
  
  // Organization
  'getorganization',
  
  // Events
  'getevents',
  'getevent',
  
  // Webhooks
  'createwebhook',
  'updatewebhook',
  'verifywebhook',
  'getwebhook',
  'getwebhooks',
  'deletewebhook',
  
  // Users
  'getuser',
  'getusers',
  
  // SAFEs
  'getsaferequests',
  'getsaferequest',
  'getsaferequestdocument',
  
  // Send Money Requests
  'getsendmoneyapprovalrequest',
  
  // OAuth2
  'startoauth2flow',
  'obtainaccesstoken'
];

function fetchEndpoint(endpoint) {
  return new Promise((resolve, reject) => {
    const url = `https://docs.mercury.com/reference/${endpoint}.md`;
    
    console.log(`Fetching ${endpoint}...`);
    
    https.get(url, (res) => {
      let data = '';
      
      res.on('data', (chunk) => {
        data += chunk;
      });
      
      res.on('end', () => {
        try {
          // Extract JSON from markdown
          const match = data.match(/```json\n([\s\S]*?)\n```/);
          if (match && match[1]) {
            const json = JSON.parse(match[1]);
            console.log(`✓ ${endpoint}`);
            resolve(json);
          } else {
            console.log(`✗ ${endpoint} - No JSON found`);
            resolve(null);
          }
        } catch (err) {
          console.log(`✗ ${endpoint} - Parse error: ${err.message}`);
          resolve(null);
        }
      });
    }).on('error', (err) => {
      console.log(`✗ ${endpoint} - Fetch error: ${err.message}`);
      resolve(null);
    });
  });
}

async function main() {
  console.log('Scraping Mercury API endpoints...\n');
  
  const results = [];
  
  // Fetch endpoints in batches to avoid overwhelming the server
  const batchSize = 5;
  for (let i = 0; i < endpoints.length; i += batchSize) {
    const batch = endpoints.slice(i, i + batchSize);
    const batchResults = await Promise.all(batch.map(fetchEndpoint));
    results.push(...batchResults);
    
    // Small delay between batches
    if (i + batchSize < endpoints.length) {
      await new Promise(resolve => setTimeout(resolve, 1000));
    }
  }
  
  // Filter out nulls
  const validResults = results.filter(r => r !== null);
  
  console.log(`\n✓ Successfully fetched ${validResults.length}/${endpoints.length} endpoints`);
  
  // Merge into complete OpenAPI spec
  const openapi = {
    openapi: '3.0.0',
    info: {
      title: 'Mercury API',
      version: '1.0.0',
      description: 'Complete Mercury Banking API specification'
    },
    servers: [
      {
        url: 'https://api.mercury.com/api/v1',
        description: 'Mercury API Production'
      }
    ],
    security: [
      { bearerAuth: [] },
      { basicAuth: [] }
    ],
    components: {
      securitySchemes: {
        basicAuth: {
          type: 'http',
          scheme: 'basic',
          description: 'Basic authentication using API token as username'
        },
        bearerAuth: {
          type: 'http',
          scheme: 'bearer',
          description: 'Bearer token authentication'
        }
      },
      schemas: {}
    },
    paths: {},
    tags: []
  };
  
  // Merge all paths and components
  const seenTags = new Set();
  
  for (const result of validResults) {
    // Merge paths
    if (result.paths) {
      Object.assign(openapi.paths, result.paths);
    }
    
    // Merge component schemas
    if (result.components && result.components.schemas) {
      Object.assign(openapi.components.schemas, result.components.schemas);
    }
    
    // Merge tags
    if (result.tags) {
      for (const tag of result.tags) {
        if (!seenTags.has(tag.name)) {
          seenTags.add(tag.name);
          openapi.tags.push(tag);
        }
      }
    }
  }
  
  // Write to file
  const outputPath = './mercury-openapi.json';
  fs.writeFileSync(outputPath, JSON.stringify(openapi, null, 2));
  
  console.log(`\n✓ Complete OpenAPI spec written to ${outputPath}`);
  console.log(`  Paths: ${Object.keys(openapi.paths).length}`);
  console.log(`  Schemas: ${Object.keys(openapi.components.schemas).length}`);
  console.log(`  Tags: ${openapi.tags.length}`);
  
  console.log('\nNow regenerate the C# client with NSwag:');
  console.log('  nswag openapi2csclient \\');
  console.log('    /input:mercury-openapi.json \\');
  console.log('    /output:../src/MercuryBankApi/Generated/MercuryApiClient.cs \\');
  console.log('    /namespace:MercuryBankApi.Generated \\');
  console.log('    /className:MercuryApiClient \\');
  console.log('    /generateClientInterfaces:true /generateDtoTypes:true \\');
  console.log('    /injectHttpClient:true /useBaseUrl:false /jsonLibrary:SystemTextJson');
}

main().catch(console.error);
